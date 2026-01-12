import type { Paginated } from "@/api/axios";
import DataTable from "@/components/data-table";
import { Button } from "@/components/ui/button";
import { ToggleGroup, ToggleGroupItem } from "@/components/ui/toggle-group";
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
  const [state, setState] = useState<string>("pessoas");
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

      <DataTable table={table} dataQuery={dataQuery} />
    </>
  )
}
