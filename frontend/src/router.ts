import { createBrowserRouter, redirect } from "react-router";

export const routes = {
  resumos: "/resumos",

  manager: {
    categorias: "/manager/categorias",
    pessoas: "/manager/pessoas",
    transacoes: "/manager/transacoes",
  },
}

export const router = createBrowserRouter([
  {
    path: "/",
    lazy: async () => ({
      Component: (await import("./layouts/DefaultLayout.tsx")).default
    }),
    children: [
      {
        index: true,
        loader: () => redirect("resumos")
      },
      {
        path: "resumos",
        lazy: async () => ({
          Component: (await import("./pages/DashboardResumos.tsx")).default
        })
      },
      {
        path: "manager",
        children: [
          {
            index: true,
            loader: () => redirect("transacoes")
          },
          {
            path: "transacoes",
            lazy: async () => ({
              Component: (await import("./pages/ManagerTransacoes.tsx")).default
            })
          },
          {
            path: "categorias",
            lazy: async () => ({
              Component: (await import("./pages/ManagerCategorias.tsx")).default
            })
          },
          {
            path: "pessoas",
            lazy: async () => ({
              Component: (await import("./pages/ManagerPessoas.tsx")).default
            })
          }
        ]
      }
    ]
  }
]);
