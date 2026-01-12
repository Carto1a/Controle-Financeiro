import { useEffect } from "react"
import { useNavigate } from "react-router"

export default function NotFound() {
  const navigate = useNavigate()

  useEffect(() => {
    const timer = setTimeout(() => {
      navigate("/", { replace: true })
    }, 2000)

    return () => clearTimeout(timer)
  }, [navigate])

  return (
    <div className="flex h-screen flex-col items-center justify-center gap-4">
      <h1 className="text-4xl font-bold">404</h1>
      <p className="text-muted-foreground">
        Página não encontrada. Redirecionando para a home…
      </p>
    </div>
  )
}
