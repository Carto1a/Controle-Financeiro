import type { Paginated } from "@/api/axios";
import DataTable from "@/components/DataTable";
import { categoriasService, type Categoria } from "@/services/categorias";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { getCoreRowModel, getPaginationRowModel, getSortedRowModel, useReactTable, type ColumnDef, type PaginationState, type SortingState } from "@tanstack/react-table";
import { useMemo, useState } from "react";

export default function ManagerCategorias() {
  const [sorting, setSorting] = useState<SortingState>([])
  const [pagination, setPagination] = useState<PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  })

  const columns: ColumnDef<Categoria[]>[] = [
    {
      header: "Valor",
      accessorKey: "value"
    }
  ]

  const defaultData = useMemo(() => [], [])

  const dataQuery = useQuery<Paginated<Categoria[]>, unknown>({
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
      <DataTable table={table} />
    </>
  )
}
