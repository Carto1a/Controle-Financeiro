import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { Button } from "@/components/ui/button"
import { Menu } from "lucide-react"
import { useNavigate } from "react-router"

export default function Navigator() {
  const navigate = useNavigate()

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button variant="ghost" size="icon">
          <Menu className="h-4 w-4" />
        </Button>
      </DropdownMenuTrigger>

      <DropdownMenuContent align="end" className="w-56">
        <DropdownMenuItem onClick={() => navigate("/resumos")}>
          Resumos
        </DropdownMenuItem>

        <DropdownMenuSeparator />

        <DropdownMenuItem onClick={() => navigate("/manager/categorias")}>
          Categorias
        </DropdownMenuItem>
        <DropdownMenuItem onClick={() => navigate("/manager/pessoas")}>
          Pessoas
        </DropdownMenuItem>
        <DropdownMenuItem onClick={() => navigate("/manager/transacoes")}>
          Transações
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  )
}
