import { useState, type ReactElement } from "react";
import { HoverCard, HoverCardContent, HoverCardTrigger } from "./ui/hover-card";

export interface HoverCardFetchProps<T> {
  label: string
  id: string
  fetch: (id: string) => Promise<T>
  render: (value: T) => ReactElement
}

export default function HoverCardFetch<T>(props: HoverCardFetchProps<T>) {
  const [detalhes, setDetalhes] = useState<T | null>(null)
  const [loading, setLoading] = useState(false)

  const fetchDetalhes = async () => {
    if (detalhes || loading) return
    setLoading(true)
    try {
      const result = await props.fetch(props.id)
      setDetalhes(result)
    } finally {
      setLoading(false)
    }
  }

  return (
    <HoverCard onOpenChange={(open) => open && fetchDetalhes()}>
      <HoverCardTrigger asChild>
        <span className="cursor-pointer underline">{props.label}</span>
      </HoverCardTrigger>

      <HoverCardContent className="w-64">
        {loading && <p>Carregando...</p>}
        {!loading && detalhes && props.render(detalhes)}
        {!loading && !detalhes && <p>Sem detalhes disponíveis</p>}
      </HoverCardContent>
    </HoverCard>
  )
}
