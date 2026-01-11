import { Button, Flex, TextField, Text } from "@radix-ui/themes";
import { z } from "zod";
import { Controller, FormProvider, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useRef, useState } from "react";
import toast from "react-hot-toast";
import { pessoaService, type CriarPessoaCommand } from "@/services/pessoas";
import { DatePicker } from "@/components/ui/datepicker";

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
              Data de Nascimento
            </Text>
            <Controller
              name="dataNascimento"
              control={methods.control}
              render={({ field }) => (
                <DatePicker
                  value={field.value}
                  onChange={field.onChange}
                />
              )}
            />
          </label>
          {methods.formState.errors.dataNascimento && (
            <span className="text-red-500 text-sm">
              {methods.formState.errors.dataNascimento.message}
            </span>
          )}
        </Flex>

        <Flex gap="3" mt="4" justify="end">
          <Button type="button" variant="soft" color="gray" onClick={handleCancel}>
            Cancelar
          </Button>
          <Button type="submit" disabled={methods.formState.isLoading} loading={loading}>Criar</Button>
        </Flex>
      </form>
    </FormProvider>
  )
}
