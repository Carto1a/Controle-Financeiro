import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useRef, useState } from "react";
import toast from "react-hot-toast";
import { pessoaService, type CriarPessoaCommand } from "@/services/pessoas";
import { DatePicker } from "@/components/datepicker";
import ManagerForm from "@/components/manager-form";
import { Input } from "@/components/ui/input";
import ControllerField from "@/components/controller-field";

const pessoaSchema = z.object({
  nome: z.string().min(1, "Nome obrigatório"),
  dataNascimento: z
    .date({ error: "Data de nascimento obrigatória" })
    .max(new Date(), "Data de nascimento esta no futuro")
});

type PessoaFormData = z.infer<typeof pessoaSchema>;

function usePessoaForm() {
  return useForm<PessoaFormData>({
    resolver: zodResolver(pessoaSchema)
  });
}

export interface ManagerPessoaFormProps { onCreated: () => void, onCancel: () => void }
export default function ManagerPessoaForm(props: ManagerPessoaFormProps) {
  const abortControllerRef = useRef<AbortController | null>(null);
  const methods = usePessoaForm();
  const [loading, setLoading] = useState(false);
  abortControllerRef.current = new AbortController();
  const signal = abortControllerRef.current.signal;

  async function onSubmit(data: PessoaFormData) {
    setLoading(true);

    let request: CriarPessoaCommand = {
      nome: data.nome,
      dataNascimento: data.dataNascimento.toISOString()
    }

    try {
      await pessoaService.criar(request, signal);

      toast.success("Pessoa criada com sucesso!");
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
      <ControllerField<PessoaFormData>
        name="nome"
        label="Nome"
        control={methods.control}
        compoment={({ value, onChange }) => (
          <Input value={value} onChange={onChange} />
        )}
      />
      <ControllerField<PessoaFormData>
        name="dataNascimento"
        label="Data de Nascimento"
        control={methods.control}
        compoment={({ value, onChange }) => (
          <DatePicker
            value={value}
            onChange={onChange}
          />
        )}
      />
    </ManagerForm>
  )
}
