import { Button, Flex } from "@radix-ui/themes";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { z } from "zod";
import { FormProvider, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { CategoriaFinalidade, categoriasService, type CriarCategoriaCommand } from "@/services/categorias";
import { useRef, useState } from "react";
import toast from "react-hot-toast";
import ControllerField from "@/components/controllerField";
import { Input } from "@/components/ui/input";
import ManagerForm from "@/components/manager-form";

const categoriaSchema = z.object({
  nome: z.string().min(1, "Nome obrigatório"),
  descricao: z.string().min(1, "Descrição obrigatória"),
  finalidade: z.string().refine(
    (v) => Object.values(CategoriaFinalidade).map(String).includes(v),
    { message: "Finalidade obrigatória" }
  ),
});

type CategoriaFormData = z.infer<typeof categoriaSchema>;

function useCategoriaForm() {
  return useForm<CategoriaFormData>({
    resolver: zodResolver(categoriaSchema),
    defaultValues: {
      finalidade: ""
    }
  });
}

export interface ManagerCategoriaFormProps { onCreated: () => void, onCancel: () => void }
export default function ManagerCategoriaForm(props: ManagerCategoriaFormProps) {
  const abortControllerRef = useRef<AbortController | null>(null);
  const methods = useCategoriaForm();
  const [loading, setLoading] = useState(false);
  abortControllerRef.current = new AbortController();
  const signal = abortControllerRef.current.signal;

  async function onSubmit(data: CategoriaFormData) {
    setLoading(true);

    try {
      let request: CriarCategoriaCommand = {
        nome: data.nome,
        descricao: data.descricao,
        finalidade: parseInt(data.finalidade) as CategoriaFinalidade
      }

      await categoriasService.criar(request, signal);

      toast.success("Categoria criada com sucesso!");
      methods.reset();
      props.onCreated?.();
    } catch (err: any) {
      if (err.name == "AbortError") {
        toast.error("Resquisição cancelada");
      } else {
        toast.error(err.message || "Erro inesperado");
      }
    } finally {
      setLoading(false);
    }
  }

  function handleCancel() {
    abortControllerRef.current?.abort();
    props.onCancel?.();
  }

  return (
    <ManagerForm
      form={methods}
      handleSubmit={onSubmit}
      loading={loading}
      handleCancel={handleCancel}
    >
      <ControllerField<CategoriaFormData>
        name="nome"
        label="Nome"
        control={methods.control}
        compoment={({ value, onChange }) => (
          <Input value={value} onChange={onChange} />
        )} />
      <ControllerField<CategoriaFormData>
        name="descricao"
        label="Descrição"
        control={methods.control}
        compoment={({ value, onChange }) => (
          <Input value={value} onChange={onChange} />
        )} />
      <ControllerField<CategoriaFormData>
        name="finalidade"
        label="Finalidade"
        control={methods.control}
        compoment={({ value, onChange }) => (
          <Select value={value} onValueChange={onChange}>
            <SelectTrigger className="!w-1/2">
              <SelectValue placeholder="Selecione a finalidade" />
            </SelectTrigger>
            <SelectContent>
              {Object.entries(CategoriaFinalidade).map(([label, value]) =>
                <SelectItem key={value} value={value.toString()}>
                  {label}
                </SelectItem>
              )}
            </SelectContent>
          </Select>
        )} />
    </ManagerForm>
  )
}
