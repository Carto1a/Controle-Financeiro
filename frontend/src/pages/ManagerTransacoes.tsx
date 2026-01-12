import type { Paginated } from "@/api/axios";
import DataTable from "@/components/data-table";
import { TipoTransacao, transacoesService, type Transacao } from "@/services/transacoes";
import { Badge, Dialog, Text } from "@radix-ui/themes";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import {
  getCoreRowModel,
  getPaginationRowModel,
  useReactTable,
  type ColumnDef,
  type PaginationState,
} from "@tanstack/react-table";
import { useMemo, useState } from "react";
import ManagerTransacaoForm from "./ManagerTransacoeForm";
import HoverCardFetch from "@/components/hover-card-fetch";
import { pessoaService, type Pessoa, type PessoaSimples } from "@/services/pessoas";
import { CategoriaFinalidade, categoriasService, type Categoria, type CategoriaSimples } from "@/services/categorias";
import { Button } from "@/components/ui/button";

export default function ManagerTransacoes() {
  const [dialogOpen, setDialogOpen] = useState(false);
  const [pagination, setPagination] = useState<PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  })

  const columns: ColumnDef<Transacao>[] = [
    {
      header: "Descrição",
      accessorKey: "descricao"
    },
    {
      header: "Valor",
      accessorKey: "valor",
      cell: info => {
        const value = info.getValue() as number
        return new Intl.NumberFormat("pt-BR", {
          style: "currency",
          currency: "BRL"
        }).format(value)
      }
    },
    {
      header: "Tipo",
      accessorKey: "tipoTransacao",
      cell: info => {
        const type = info.getValue() as TipoTransacao
        const color = type === TipoTransacao.despesa ? "red" : type === TipoTransacao.receita ? "green" : "gray"
        return (
          <Badge variant="soft" radius="full" color={color}>
            {Object.entries(TipoTransacao).find(([_, value]) => value === type)?.[0] ?? "desconhecido"}
          </Badge>
        )
      }
    },
    {
      header: "Pessoa",
      accessorKey: "pessoa",
      cell: info => {
        const pessoaSimples = info.getValue() as PessoaSimples;
        if (!pessoaSimples) {
          return <Text color="gray">—</Text>
        }

        return (
          <HoverCardFetch<Pessoa>
            id={pessoaSimples.id}
            label={pessoaSimples.nome}
            fetch={pessoaService.buscarPorId}
            render={(pessoa) => (
              <div className="flex flex-col gap-1 p-3">
                <Text className="font-semibold text-gray-900 dark:text-gray-100 text-sm">
                  {pessoa.nome}
                </Text>
                <Text className="text-gray-500 dark:text-gray-400 text-xs">
                  Nascimento: {new Date(pessoa.dataNascimento).toLocaleDateString()}
                </Text>
              </div>
            )}
          />
        )
      }
    },
    {
      header: "Categoria",
      accessorKey: "categoria",
      cell: info => {
        const categoriaSimples = info.getValue() as CategoriaSimples;
        if (!categoriaSimples) {
          return <Text color="gray">—</Text>
        }

        return (
          <HoverCardFetch<Categoria>
            id={categoriaSimples.id}
            label={categoriaSimples.nome}
            fetch={categoriasService.buscarPorId}
            render={(categoria) => {
              const type = categoria.finalidade;

              const color =
                type === CategoriaFinalidade.despesa
                  ? "red"
                  : type === CategoriaFinalidade.receita
                    ? "green"
                    : "blue";

              const label =
                Object.entries(CategoriaFinalidade)
                  .find(([_, value]) => value === type)?.[0] ??
                "desconhecido";

              return (
                <div className="flex flex-col gap-1 p-3">
                  <Text className="font-semibold text-gray-900 dark:text-gray-100 text-sm">
                    {categoria.nome}
                  </Text>
                  <Badge
                    variant="soft"
                    radius="full"
                    color={color}
                    className="!px-3 !text-sm !rounded-lg w-fit"
                  >
                    {label}
                  </Badge>
                  <Text className="text-gray-500 dark:text-gray-400 text-xs">
                    {categoria.descricao}
                  </Text>
                </div>
              )
            }}
          />
        )
      }
    },
    {
      header: "Data",
      accessorKey: "data",
      cell: info => {
        const date = new Date(info.getValue() as string)
        return date.toLocaleString()
      }
    },
    {
      header: "Criado Em",
      accessorKey: "criadoEm",
      cell: info => {
        const date = new Date(info.getValue() as string)
        return date.toLocaleString()
      }
    }
  ]

  const defaultData = useMemo(() => [], [])

  const dataQuery = useQuery<Paginated<Transacao>, unknown>({
    queryKey: ['data', pagination],
    queryFn: () => transacoesService.listarDetalhado({ pagina: pagination.pageIndex, tamanhoPagina: pagination.pageSize }),
    placeholderData: keepPreviousData,
  })

  const table = useReactTable({
    data: dataQuery.data?.items ?? defaultData,
    columns,
    rowCount: dataQuery.data?.total ?? 0,
    manualSorting: true,
    manualPagination: true,
    onPaginationChange: setPagination,
    getCoreRowModel: getCoreRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    state: {
      pagination
    },
  })

  return (
    <>
      <div className="flex flex-col items-end">
        <Dialog.Root open={dialogOpen} onOpenChange={setDialogOpen}>
          <Dialog.Trigger>
            <Button className="!min-w-30 bg-green-500 text-white hover:bg-green-600">
              Criar
            </Button>
          </Dialog.Trigger>
          <Dialog.Content maxWidth="500px">
            <Dialog.Title>Nova Transacao</Dialog.Title>
            <ManagerTransacaoForm
              onCreated={() => setDialogOpen(false)}
              onCancel={() => setDialogOpen(false)}
            />
          </Dialog.Content>
        </Dialog.Root>

      </div>
      <DataTable table={table} dataQuery={dataQuery} />
    </>
  )
}
