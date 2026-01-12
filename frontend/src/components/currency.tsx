import { formatCurrency } from "@/helpers";

export interface CurrencyProps { value: number };
export default function Currency(props: CurrencyProps) {
  const colorClass = props.value > 0
    ? "text-green-600"
    : props.value < 0
      ? "text-red-600"
      : "text-gray-500";
  return (
    <span className={colorClass}>
      {formatCurrency(props.value)}
    </span>
  );
}
