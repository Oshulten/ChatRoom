/* eslint-disable react/react-in-jsx-scope */
import { QueryClient } from '@tanstack/react-query'
import { ReactQueryDevtools } from '@tanstack/react-query-devtools'
import { createRootRouteWithContext, Navigate, Outlet } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/router-devtools'
import './../css/__root.css'

export const Route = createRootRouteWithContext<{ queryClient: QueryClient }>()({
    component: RootComponent,
});

function RootComponent() {
    return (
        <>
            <div className="w-full bg-slate-400 flex flex-col">
                <h1>Chat Spaces</h1>
                <Navigate to="/login" />
                <Outlet />
            </div>
            <ReactQueryDevtools buttonPosition="top-right" />
            <TanStackRouterDevtools position="bottom-right" />
        </>
    )
}
