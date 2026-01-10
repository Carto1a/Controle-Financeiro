import { DropdownMenu, IconButton } from "@radix-ui/themes";
import { RxHamburgerMenu } from "react-icons/rx";
import { useNavigate } from "react-router";

export default function Navigator() {
  const navigate = useNavigate();

  return (
    <DropdownMenu.Root>
      <DropdownMenu.Trigger>
        <IconButton>
          <RxHamburgerMenu />
        </IconButton>
      </DropdownMenu.Trigger>
      <DropdownMenu.Content size="2">
        <DropdownMenu.Item onClick={() => navigate("/resumos")}>Resumos</DropdownMenu.Item>
        <DropdownMenu.Separator />
        <DropdownMenu.Item onClick={() => navigate("/manager/categorias")}>Categorias</DropdownMenu.Item>
        <DropdownMenu.Item onClick={() => navigate("/manager/pessoas")}>Pessoas</DropdownMenu.Item>
        <DropdownMenu.Item onClick={() => navigate("/manager/transacoes")}>Transacoes</DropdownMenu.Item>
      </DropdownMenu.Content>
    </DropdownMenu.Root>
  )
}
