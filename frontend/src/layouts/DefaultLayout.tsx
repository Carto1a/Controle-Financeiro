import { Text } from "@radix-ui/themes";
import { Outlet } from "react-router";
import Navigator from "../components/navigator";
import { useRouteMeta } from "@/hooks/useRouteMeta";
import type { RouteMetadata } from "@/router";

export default function DefaultLayout() {
  const routeMeta = useRouteMeta<RouteMetadata>();

  return (
    <div className="flex flex-col h-screen bg-gray-100 p-[1rem] gap-4">
      <div className="relative flex items-center">
        <div className="flex-shrink-0">
          <Navigator />
        </div>

        {routeMeta.title && (
          <Text className="absolute left-1/2 -translate-x-1/2 text-xl font-semibold ">
            {routeMeta.title}
          </Text>
        )}
      </div>
      <Outlet />
    </div>
  )
}
