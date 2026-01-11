import { IconButton, Table, Text } from "@radix-ui/themes"
import { ChevronRight, ChevronLeft } from 'lucide-react';
import { flexRender, } from "@tanstack/react-table"

export interface DataTableProps<TData> { table: import("@tanstack/table-core").Table<TData> }
export default function DataTable<TData extends unknown>(props: DataTableProps<TData>) {
  return (
    <div className="flex flex-col justify-between h-full">
      <Table.Root size={"3"} layout={"fixed"}>
        <Table.Header>
          {
            props.table.getHeaderGroups().map(headerGroup => (
              <Table.Row key={headerGroup.id}>
                {headerGroup.headers.map(header => {
                  return (
                    <Table.ColumnHeaderCell key={header.id}>
                      {header.isPlaceholder
                        ? null
                        : flexRender(
                          header.column.columnDef.header,
                          header.getContext())}
                    </Table.ColumnHeaderCell>
                  )
                })}
              </Table.Row>
            ))}
        </Table.Header>
        <Table.Body>
          {props.table.getRowModel().rows?.length ? (
            props.table.getRowModel().rows.map(row => (
              <Table.Row key={row.id}>
                {row._getAllVisibleCells().map(cell => (
                  <Table.Cell key={cell.id}>
                    {flexRender(
                      cell.column.columnDef.cell,
                      cell.getContext()
                    )}
                  </Table.Cell>
                ))}
              </Table.Row>
            ))
          ) : (
            <Table.Row>
              <Table.Cell
                colSpan={props.table.getAllColumns().length}
              >
                Sem Resultados
              </Table.Cell>
            </Table.Row>
          )}
        </Table.Body>
      </Table.Root>

      <div className="flex items-center justify-between gap-4 bg-gray-100 rounded-md">
        <Text className="text-md font-medium text-gray-700">
          Total de items: {props.table.getRowCount()}
        </Text>

        <div className="flex items-center gap-2">
          <IconButton
            className="p-2 bg-white rounded hover:bg-gray-200 disabled:opacity-50"
            disabled={!props.table.getCanPreviousPage()}
            onClick={() => props.table.previousPage()}
          >
            <ChevronLeft size={18} />
          </IconButton>

          <Text className="text-md font-medium text-gray-700">
            {props.table.getPageCount()}
          </Text>

          <IconButton
            className="p-2 bg-white rounded hover:bg-gray-200 disabled:opacity-50"
            disabled={!props.table.getCanNextPage()}
            onClick={() => props.table.nextPage()}
          >
            <ChevronRight size={18} />
          </IconButton>
        </div>
      </div>
    </div>
  )
}
