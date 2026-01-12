import type { Paginated } from "@/api/axios";
import DataTable from "@/components/data-table";
import { CategoriaFinalidade, categoriasService, type Categoria } from "@/services/categorias";
import { Badge, Button, Dialog } from "@radix-ui/themes";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import {
  getCoreRowModel,
  getPaginationRowModel,
  useReactTable,
  type ColumnDef,
  type PaginationState
} from "@tanstack/react-table";
import { useMemo, useState } from "react";
import ManagerCategoriaForm from "./ManagerCategoriaForm";

export default function ManagerCategorias() {
  const [dialogOpen, setDialogOpen] = useState(false);
  const [pagination, setPagination] = useState<PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  })

  const columns: ColumnDef<Categoria>[] = [
    {
      header: "Nome",
      accessorKey: "nome"
    },
    {
      header: "Descrição",
      accessorKey: "descricao"
    },
    {
      header: "finalidade",
      accessorKey: "finalidade",
      cell: info => {
        const type = info.getValue() as CategoriaFinalidade
        const color = type === CategoriaFinalidade.despesa ? "red" : type === CategoriaFinalidade.receita ? "green" : "blue"
        return (
          <Badge variant="soft" radius="full" color={color}>
            {Object.entries(CategoriaFinalidade).find(([_, value]) => value === type)?.[0] ?? "desconhecido"}
          </Badge>
        )
      }
    }
  ]

  const defaultData = useMemo(() => [], [])

  const dataQuery = useQuery<Paginated<Categoria>, unknown>({
    queryKey: ['data', pagination],
    queryFn: () => categoriasService.listarDetalhado({ pagina: pagination.pageIndex, tamanhoPagina: pagination.pageSize }),
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
      <DataTable table={table} dataQuery={dataQuery} />
    </>
  )
}
