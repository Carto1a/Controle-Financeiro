import { Outlet } from "react-router";
import Navigator from "../components/Navigator";
import { Box, Container, Flex, Section } from "@radix-ui/themes";
import DataTable from "../components/DataTable";
import {
  getCoreRowModel,
  getFilteredRowModel,
  getPaginationRowModel,
  getSortedRowModel,
  useReactTable,
  type ColumnDef,
  type ColumnFiltersState,
  type PaginationState,
  type SortingState,
  type VisibilityState,
} from "@tanstack/react-table"
import {
  keepPreviousData,
  useQuery,
} from '@tanstack/react-query'
import React, { useMemo, useState } from "react";
import type { Paginated } from "../api/axios";
import type { Categoria } from "../services/categorias";

export default function DefaultLayout() {
  const data = [
    {
      value: 10
    },
    {
      value: 10
    },
    {
      value: 10
    },
    {
      value: 10
    },
    {
      value: 10
    },
    {
      value: 10
    },
    {
      value: 10
    },
    {
      value: 10
    },
    {
      value: 11
    },
    {
      value: 10
    },
    {
      value: 10
    },
    {
      value: 10
    },
    {
      value: 10
    },
    {
      value: 10
    },
    {
      value: 10
    },
    {
      value: 10
    },
    {
      value: 10
    },
  ]


  const columns: ColumnDef<unknown>[] = [
    {
      header: "Valor",
      accessorKey: "value"
    }
  ]

  const [sorting, setSorting] = useState<SortingState>([])
  const [pagination, setPagination] = useState<PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  })

  const dataQuery = useQuery<void, void, Paginated<Categoria[]>>({
    queryKey: ['data', pagination],
    queryFn: () => console.log("teste"),
    placeholderData: keepPreviousData, // don't have 0 rows flash while changing pages/loading next page
  })

  const defaultData = useMemo(() => [], [])

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
    <Box p={{ sm: '4' }}>
      <Navigator />

      <Box width={{ sm: "100%", md: "100%", lg: "100%" }}>
        <DataTable table={table} />
      </Box>


      <Outlet />
    </Box>
  )
}
