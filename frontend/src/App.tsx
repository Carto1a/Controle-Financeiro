import { RouterProvider } from "react-router/dom";
import { router } from "./router";
import { Theme } from "@radix-ui/themes";
import "./assets/styles/reset.css"
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

function App() {
  const queryClient = new QueryClient();

  return (
    <Theme accentColor='crimson' grayColor='sand' radius='large'>
      <QueryClientProvider client={queryClient}>
        <RouterProvider router={router} />
      </QueryClientProvider>
    </Theme>
  )
}

export default App
