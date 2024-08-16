/* eslint-disable react/react-in-jsx-scope */
import { QueryClient } from '@tanstack/react-query'
import { ReactQueryDevtools } from '@tanstack/react-query-devtools'
import { createRootRouteWithContext, Link, Navigate, Outlet } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/router-devtools'

export const Route = createRootRouteWithContext<{ queryClient: QueryClient }>()({
    component: RootComponent,
})

function RootComponent() {
    return (
        <>
            <div className="p-2 flex gap-2 text-lg">
                <Link
                    to={'/login'}
                    activeProps={{
                        className: 'font-bold',
                    }}
                >
                    Login
                </Link>{' '}
                <Link
                    to="/createAccount"
                    activeProps={{
                        className: 'font-bold',
                    }}
                >
                    Create Account
                </Link>{' '}
            </div>
            <hr />
            <Navigate from={Route.fullPath} to={"/login"} />
            <Outlet />
            <ReactQueryDevtools buttonPosition="top-right" />
            <TanStackRouterDevtools position="bottom-right" />
        </>
    )
}
