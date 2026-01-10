import { Table } from "@radix-ui/themes"
import { flexRender, } from "@tanstack/react-table"

export interface DataTableProps<TData> { table: import("@tanstack/table-core").Table<TData> }
export default function DataTable<TData extends unknown>(props: DataTableProps<TData>) {
  return (
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
                        header.getContext()
                      )}
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
  )
}
