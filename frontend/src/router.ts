import { createBrowserRouter, redirect } from "react-router";

export interface RouteMetadata {
  title?: string;
}

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
        handle: {
          title: "Resumos"
        },
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
            handle: {
              title: "Transacoes"
            },
            lazy: async () => ({
              Component: (await import("./pages/ManagerTransacoes.tsx")).default
            })
          },
          {
            path: "categorias",
            handle: {
              title: "Categorias"
            },
            lazy: async () => ({
              Component: (await import("./pages/ManagerCategorias.tsx")).default
            })
          },
          {
            path: "pessoas",
            handle: {
              title: "Pessoas"
            },
            lazy: async () => ({
              Component: (await import("./pages/ManagerPessoas.tsx")).default
            })
          }
        ]
      }
    ]
  }
]);
