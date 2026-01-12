import type { Paginated } from "@/api/axios";
import DataTable from "@/components/data-table";
import { resumosService, type ResumoFinanceiro } from "@/services/resumos";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import {
  getCoreRowModel,
  getPaginationRowModel,
  useReactTable,
  type ColumnDef,
  type PaginationState
} from "@tanstack/react-table";
import { useMemo, useState } from "react";

export default function DashboardResumos() {
  const [pagination, setPagination] = useState<PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  })

  const columns: ColumnDef<ResumoFinanceiro>[] = [
    {
      header: "Nome",
      accessorKey: "nome"
    },
    {
      header: "Total Receitas",
      accessorKey: "totalReceitas"
    },
    {
      header: "Total Despesas",
      accessorKey: "totalDespesas"
    },
    {
      header: "Saldo",
      accessorKey: "saldo",
      cell: info => {
        const value = info.getValue() as number;

        const colorClass = value > 0
          ? "text-green-600"
          : value < 0
            ? "text-red-600"
            : "text-gray-500";

        return (
          <span className={colorClass}>
            {new Intl.NumberFormat("pt-BR", {
              style: "currency",
              currency: "BRL"
            }).format(value)}
          </span>
        );
      }
    }
  ]

  const defaultData = useMemo(() => [], [])

  const dataQuery = useQuery<Paginated<ResumoFinanceiro>, unknown>({
    queryKey: ['data', pagination],
    queryFn: () => resumosService.porCategoria({ pagina: pagination.pageIndex, tamanhoPagina: pagination.pageSize }),
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
      <DataTable table={table} dataQuery={dataQuery} />
    </>
  )
}
