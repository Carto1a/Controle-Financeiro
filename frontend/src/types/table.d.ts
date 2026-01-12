import "@tanstack/react-table"

export interface TotalMeta<TData, TValue> {
  totalKey?: keyof any
  format?: (value: TValue) => React.ReactNode
}

declare module "@tanstack/react-table" {
  interface ColumnMeta<TData, TValue> {
    total?: TotalMeta
  }
}

