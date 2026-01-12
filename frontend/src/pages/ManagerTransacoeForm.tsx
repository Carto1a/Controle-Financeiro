import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect, useRef, useState } from "react";
import toast from "react-hot-toast";
import { TipoTransacao, transacoesService, type CriarTransacaoCommand } from "@/services/transacoes";
import ManagerForm from "@/components/manager-form";
import ControllerField from "@/components/controller-field";
import { Input } from "@/components/ui/input";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { pessoaService, type PessoaSimples } from "@/services/pessoas";
import { categoriasService, type CategoriaSimples } from "@/services/categorias";
import { NumberInput } from "@/components/input-number";
import { DateTimePicker } from "@/components/date-time-picker";

const transacaoSchema = z.object({
  descricao: z.string().min(1, "Descrição obrigatória"),
  valor: z.number().positive("Valor deve ser maior que 0"),
  pessoaId: z.uuid({ error: "Id de Pessoa inválido" }),
  categoriaId: z.uuid({ error: "Id de Categoria inválido" }),
  data: z.date().optional(),
  tipo: z.string().refine(
    (v) => Object.values(TipoTransacao).map(String).includes(v),
    { message: "Tipo obrigatório" }
  ),
});

type TransacaoFormData = z.infer<typeof transacaoSchema>;

function useTransacaoForm() {
  return useForm<TransacaoFormData>({
    resolver: zodResolver(transacaoSchema),
    defaultValues: {
      tipo: "",
      valor: 0
    }
  });
}

export interface ManagerTransacaoFormProps { onCreated: () => void, onCancel: () => void }
export default function ManagerTransacaoForm(props: ManagerTransacaoFormProps) {
  const abortControllerRef = useRef<AbortController | null>(null);
  const methods = useTransacaoForm();
  const [loading, setLoading] = useState(false);
  const [loadingPessoas, setLoadingPessoas] = useState(false);
  const [pessoas, setPessoas] = useState([] as PessoaSimples[]);
  const [loadingCategorias, setLoadingCategorias] = useState(false);
  const [categorias, setCategorias] = useState([] as CategoriaSimples[]);
  abortControllerRef.current = new AbortController();
  const signal = abortControllerRef.current.signal;

  async function getPessoas() {
    setLoadingPessoas(true);

    try {
      setPessoas(await pessoaService.listar());
    } catch (_) {
      toast.error("Erro ao buscar pessoas");
    } finally {
      setLoadingPessoas(false);
    }
  }

  async function getCategorias() {
    setLoadingCategorias(true);

    try {
      setCategorias(await categoriasService.listar());
    } catch (_) {
      toast.error("Erro ao buscar categorias");
    } finally {
      setLoadingCategorias(false);
    }
  }

  async function onSubmit(data: TransacaoFormData) {
    setLoading(true);

    let request: CriarTransacaoCommand = {
      descricao: data.descricao,
      valor: data.valor,
      pessoaId: data.pessoaId,
      categoriaId: data.categoriaId,
      data: data.data?.toISOString(),
      tipo: parseInt(data.tipo) as TipoTransacao
    }

    try {
      await transacoesService.criar(request, signal);

      toast.success("Transacao criada com sucesso!");
      methods.reset();
      props.onCreated?.();
    } catch (err: any) {
      if (err.name == "AbortError") {
        toast.error("Resquisição cancelada");
      } else {
        toast.error(err.response.data[0].message || "Erro inesperado");
      }
    } finally {
      setLoading(false);
    }
  }

  function handleCancel() {
    abortControllerRef.current?.abort();
    props.onCancel?.();
  }

  useEffect(() => {
    getPessoas();
    getCategorias();
  }, []);

  return (
    <ManagerForm
      form={methods}
      handleSubmit={onSubmit}
      loading={loading}
      handleCancel={handleCancel}
    >
      <ControllerField<TransacaoFormData>
        name="descricao"
        label="Descrição"
        control={methods.control}
        compoment={({ value, onChange }) => (
          <Input value={value} onChange={onChange} />
        )}
      />

      <ControllerField<TransacaoFormData>
        name="valor"
        label="Valor"
        control={methods.control}
        compoment={({ value, onChange }) => (
          <NumberInput
            disableStepper
            decimalScale={2}
            thousandSeparator={','}
            value={value}
            onValueChange={onChange}
          />
        )}
      />

      <ControllerField<TransacaoFormData>
        name="pessoaId"
        label="Pessoa"
        control={methods.control}
        compoment={({ value, onChange }) => (
          <Select value={value} onValueChange={onChange}>
            <SelectTrigger loading={loadingPessoas} className="!w-1/2">
              <SelectValue placeholder={
                !loadingCategorias && categorias.length == 0 ?
                  "Não existe pessoas" : "Selecione uma pessoa"}
              />
            </SelectTrigger>
            <SelectContent>
              {pessoas.map(value =>
                <SelectItem key={value.id} value={value.id}>
                  {value.nome}
                </SelectItem>
              )}
            </SelectContent>
          </Select>
        )}
      />

      <ControllerField<TransacaoFormData>
        name="categoriaId"
        label="Categoria"
        control={methods.control}
        compoment={({ value, onChange }) => (
          <Select value={value} onValueChange={onChange}>
            <SelectTrigger loading={loadingCategorias} className="!w-1/2">
              <SelectValue placeholder={
                !loadingCategorias && categorias.length == 0 ?
                  "Não existe categorias" : "Selecione uma categoria"}
              />
            </SelectTrigger>
            <SelectContent>
              {categorias.map(value =>
                <SelectItem key={value.id} value={value.id}>
                  {value.nome}
                </SelectItem>
              )}
            </SelectContent>
          </Select>
        )}
      />

      <ControllerField<TransacaoFormData>
        name="data"
        label="Data"
        control={methods.control}
        compoment={({ value, onChange }) => (
          <DateTimePicker value={value} onChange={onChange} />
        )}
      />

      <ControllerField<TransacaoFormData>
        name="tipo"
        label="Tipo"
        control={methods.control}
        compoment={({ value, onChange }) => (
          <Select value={value} onValueChange={onChange}>
            <SelectTrigger className="!w-1/2">
              <SelectValue placeholder="Selecione um tipo" />
            </SelectTrigger>
            <SelectContent>
              {Object.entries(TipoTransacao).map(([label, value]) =>
                <SelectItem key={value} value={value.toString()}>
                  {label}
                </SelectItem>
              )}
            </SelectContent>
          </Select>
        )}
      />
    </ManagerForm>
  )
}
