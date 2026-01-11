import { Button, Flex, TextField, Text, Select } from "@radix-ui/themes";
import { z } from "zod";
import { Controller, FormProvider, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { CategoriaFinalidade, categoriasService, type CriarCategoriaCommand } from "@/services/categorias";
import { useRef, useState } from "react";
import toast from "react-hot-toast";

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
    <FormProvider {...methods}>
      <form onSubmit={methods.handleSubmit(onSubmit)}>
        <Flex direction="column" gap="3">
          <label>
            <Text as="div" size="2" mb="1" weight="bold">
              Nome
            </Text>
            <TextField.Root {...methods.register("nome")} />
          </label>
          {methods.formState.errors.nome && (
            <span className="text-red-500 text-sm">
              {methods.formState.errors.nome.message}
            </span>
          )}
          <label>
            <Text as="div" size="2" mb="1" weight="bold">
              Descrição
            </Text>
            <TextField.Root {...methods.register("descricao")} />
          </label>
          {methods.formState.errors.descricao && (
            <span className="text-red-500 text-sm">
              {methods.formState.errors.descricao.message}
            </span>
          )}
          <label>
            <Text as="div" size="2" mb="1" weight="bold">
              Nome
            </Text>
            <Controller
              name="finalidade"
              control={methods.control}
              render={({ field }) => (
                <Select.Root value={field.value} onValueChange={field.onChange}>
                  <Select.Trigger placeholder="Selecione a finalidade" />
                  <Select.Content>
                    {Object.entries(CategoriaFinalidade).map(([label, value]) =>
                      <Select.Item key={value} value={value.toString()}>{label}</Select.Item>
                    )}
                  </Select.Content>
                </Select.Root>
              )}
            />
          </label>
          {methods.formState.errors.finalidade && (
            <span className="text-red-500 text-sm">
              {methods.formState.errors.finalidade.message}
            </span>
          )}
        </Flex>

        <Flex gap="3" mt="4" justify="end">
          <Button type="button" variant="soft" color="gray" onClick={handleCancel}>
            Cancelar
          </Button>
          <Button type="submit" disabled={!methods.formState.isValid} loading={loading}>Criar</Button>
        </Flex>
      </form>
    </FormProvider>
  )
}
