import type { Paginated } from "@/api/axios";
import DataTable from "@/components/data-table";
import { categoriasService, type Categoria } from "@/services/categorias";
import { Button, Dialog } from "@radix-ui/themes";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { getCoreRowModel, getPaginationRowModel, getSortedRowModel, useReactTable, type ColumnDef, type PaginationState, type SortingState } from "@tanstack/react-table";
import { useMemo, useState } from "react";
import ManagerCategoriaForm from "./ManagerCategoriaForm";

export default function ManagerCategorias() {
  const [dialogOpen, setDialogOpen] = useState(false);
  const [sorting, setSorting] = useState<SortingState>([])
  const [pagination, setPagination] = useState<PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  })

  const columns: ColumnDef<Categoria>[] = [
    {
      header: "Valor",
      accessorKey: "value"
    }
  ]

  const defaultData = useMemo(() => [], [])

  const dataQuery = useQuery<Paginated<Categoria>, unknown>({
    queryKey: ['data', pagination],
    queryFn: () => categoriasService.listarDetalhado(),
    placeholderData: keepPreviousData,
  })

  const table = useReactTable({
    data: dataQuery.data?.items ?? defaultData,
    columns,
    rowCount: dataQuery.data?.pageSize,
    manualSorting: true,
    manualPagination: true,
    onSortingChange: setSorting,
    onPaginationChange: setPagination,
    getCoreRowModel: getCoreRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    getSortedRowModel: getSortedRowModel(),
    state: {
      sorting
    },
  })

  return (
    <>
      <div className="flex flex-col items-end">
        <Dialog.Root open={dialogOpen} onOpenChange={setDialogOpen}>
          <Dialog.Trigger>
            <Button className="!min-w-30">
              Criar
            </Button>
          </Dialog.Trigger>
          <Dialog.Content maxWidth="500px">
            <Dialog.Title>Nova Categoria</Dialog.Title>
            <ManagerCategoriaForm
              onCreated={() => setDialogOpen(false)}
              onCancel={() => setDialogOpen(false)}
            />
          </Dialog.Content>
        </Dialog.Root>

      </div>
      <DataTable table={table} />
    </>
  )
}
