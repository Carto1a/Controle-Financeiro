import {
  Table,
  TableBody,
  TableCell,
  TableFooter,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"
import { Button } from "@/components/ui/button"
import { ChevronLeft, ChevronRight } from "lucide-react"
import { flexRender } from "@tanstack/react-table"
import type { UseQueryResult } from "@tanstack/react-query"
import type { Paginated } from "@/api/axios"

export interface DataTableProps<TData, TTotal> {
  table: import("@tanstack/table-core").Table<TData>
  dataQuery: UseQueryResult<Paginated<TData>, unknown>
  totalQuery?: UseQueryResult<TTotal, unknown>
}

export default function DataTable<TData, TTotal>(props: DataTableProps<TData, TTotal>) {
  const hasTotals = props.table
    .getAllColumns()
    .some(col => col.columnDef.meta?.total);

  return (
    <div className="flex flex-col justify-between h-full gap-4 rounded-md overflow-auto border">
      <div className="flex flex-col h-full overflow-auto">
        <div className="relative h-full">
          <Table>
            <TableHeader className="sticky top-0 z-10 bg-gray-100">
              {props.table.getHeaderGroups().map(headerGroup => (
                <TableRow key={headerGroup.id}>
                  {headerGroup.headers.map(header => (
                    <TableHead key={header.id}>
                      {flexRender(
                        header.column.columnDef.header,
                        header.getContext()
                      )}
                    </TableHead>
                  ))}
                </TableRow>
              ))}
            </TableHeader>

            <TableBody className="border-b">
              {props.table.getRowModel().rows.map(row => (
                <TableRow key={row.id} className="h-auto">
                  {row.getVisibleCells().map(cell => (
                    <TableCell key={cell.id}>
                      {flexRender(
                        cell.column.columnDef.cell,
                        cell.getContext()
                      )}
                    </TableCell>
                  ))}
                </TableRow>
              ))}
            </TableBody>

            {hasTotals && (
              <TableFooter className="sticky bottom-0 z-10 bg-gray-100 border-t border-b">
                <TableRow>
                  {props.table.getAllColumns().map(column => {
                    const meta = column.columnDef.meta?.total
                    if (!meta?.totalKey) return <TableCell key={column.id} />

                    const value =
                      props.totalQuery?.data?.[meta.totalKey as keyof TTotal]

                    return (
                      <TableCell key={column.id} className="font-semibold">
                        {value != null ? meta.format?.(value) ?? value : ""}
                      </TableCell>
                    )
                  })}
                </TableRow>
              </TableFooter>
            )}
          </Table>
        </div>
      </div>

      <div className="flex items-center justify-between rounded-md bg-muted px-4 py-2">
        <span className="text-sm text-muted-foreground">
          Total de itens: {props.dataQuery.data?.total ?? 0}
        </span>

        <div className="flex items-center gap-2">
          <Button
            variant="outline"
            size="icon"
            disabled={!props.table.getCanPreviousPage()}
            onClick={() => props.table.previousPage()}
          >
            <ChevronLeft className="h-4 w-4" />
          </Button>

          <span className="text-sm font-medium">
            {props.table.getState().pagination.pageIndex + 1}
          </span>

          <Button
            variant="outline"
            size="icon"
            disabled={!props.table.getCanNextPage()}
            onClick={() => props.table.nextPage()}
          >
            <ChevronRight className="h-4 w-4" />
          </Button>
        </div>
      </div>
    </div>
  )
}
