import type { Paginated } from "@/api/axios";
import DataTable from "@/components/data-table";
import { Dialog } from "@radix-ui/themes";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { getCoreRowModel, getPaginationRowModel, useReactTable, type ColumnDef, type PaginationState } from "@tanstack/react-table";
import { useMemo, useState } from "react";
import ManagerPessoaForm from "./ManagerPessoaForm";
import { pessoaService, type Pessoa } from "@/services/pessoas";
import { Button } from "@/components/ui/button";

export default function ManagerPessoas() {
  const [dialogOpen, setDialogOpen] = useState(false);
  const [pagination, setPagination] = useState<PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  })

  const columns: ColumnDef<Pessoa>[] = [
    {
      header: "Nome",
      accessorKey: "nome"
    },
    {
      header: "Data de Nascimento",
      accessorKey: "dataNascimento",
      cell: info => {
        const date = new Date(info.getValue() as string)
        return date.toLocaleDateString()
      }
    }
  ]

  const defaultData = useMemo(() => [], [])

  const dataQuery = useQuery<Paginated<Pessoa>, unknown>({
    queryKey: ['data', pagination],
    queryFn: () => pessoaService.listarDetalhado({ pagina: pagination.pageIndex, tamanhoPagina: pagination.pageSize }),
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
            <Dialog.Title>Nova Pessoa</Dialog.Title>
            <ManagerPessoaForm
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
