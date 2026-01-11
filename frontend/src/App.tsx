import { RouterProvider } from "react-router/dom";
import { router } from "./router";
import "./assets/styles/main.css"
import "@radix-ui/themes/styles.css";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Theme } from "@radix-ui/themes";

function App() {
  const queryClient = new QueryClient();

  return (
    <Theme accentColor="crimson">
      <QueryClientProvider client={queryClient}>
        <RouterProvider router={router} />
      </QueryClientProvider>
    </Theme>
  )
}

export default App
