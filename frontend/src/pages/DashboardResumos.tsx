import type { Paginated } from "@/api/axios";
import Currency from "@/components/currency";
import DataTable from "@/components/data-table";
import { Button } from "@/components/ui/button";
import { ToggleGroup, ToggleGroupItem } from "@/components/ui/toggle-group";
import { resumosService, type ResumoFinanceiro, type ResumoFinanceiroTotal } from "@/services/resumos";
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
  const [state, setState] = useState<string>("pessoas");
  const [pagination, setPagination] = useState<PaginationState>({
    pageIndex: 0,
    pageSize: 50,
  })

  const columns: ColumnDef<ResumoFinanceiro>[] = [
    {
      header: "Nome",
      accessorKey: "nome"
    },
    {
      header: "Total Receitas",
      accessorKey: "totalReceitas",
      cell: info => (
        <Currency value={info.getValue() as number} />
      ),
      meta: {
        total: {
          totalKey: "totalReceitas",
          format: (value: number) => (
            <Currency value={value} />
          )
        }
      }
    },
    {
      header: "Total Despesas",
      accessorKey: "totalDespesas",
      cell: info => (
        <Currency value={info.getValue() as number} />
      ),
      meta: {
        total: {
          totalKey: "totalDespesas",
          format: (value: number) => (
            <Currency value={value} />
          )
        }
      }
    },
    {
      header: "Saldo",
      accessorKey: "saldo",
      cell: info => (
        <Currency value={info.getValue() as number} />
      ),
      meta: {
        total: {
          totalKey: "saldo",
          format: (value: number) => (
            <Currency value={value} />
          )
        }
      }
    }
  ]

  const defaultData = useMemo(() => [], [])

  const dataQuery = useQuery<Paginated<ResumoFinanceiro>>({
    queryKey: ["resumos", state, pagination],
    queryFn: () => {
      const query = {
        pagina: pagination.pageIndex,
        tamanhoPagina: pagination.pageSize,
      };

      return state === "pessoas"
        ? resumosService.porPessoa(query)
        : resumosService.porCategoria(query);
    },
    placeholderData: keepPreviousData,
  })

  const totalQuery = useQuery<ResumoFinanceiroTotal>({
    queryKey: ["resumos-totals"],
    queryFn: () => resumosService.totalFinanceiro()
  });

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
      <div className="flex justify-center mb-4">
        <ToggleGroup
          type="single"
          value={state}
          onValueChange={setState}
        >
          <ToggleGroupItem value="pessoas" asChild>
            <Button
              variant="outline"
              className="data-[state=on]:bg-gray-600 data-[state=on]:text-white"
            >
              Pessoas
            </Button>
          </ToggleGroupItem>

          <ToggleGroupItem value="categorias" asChild>
            <Button
              variant="outline"
              className="data-[state=on]:bg-gray-600 data-[state=on]:text-white"
            >
              Categorias
            </Button>
          </ToggleGroupItem>
        </ToggleGroup>
      </div>

      <DataTable table={table} dataQuery={dataQuery} totalQuery={totalQuery} />
    </>
  )
}
