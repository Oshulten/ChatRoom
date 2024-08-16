/* eslint-disable react/react-in-jsx-scope */
import { createContext, StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

import { routeTree } from './routeTree.gen'
import { createRouter, RouterProvider } from '@tanstack/react-router';
import { DefaultCatchBoundary } from './components/DefaultCatchBoundary.tsx';
import { NotFound } from './components/NotFound.tsx';
import User from './types/user.ts';

const queryClient = new QueryClient();

const router = createRouter({
  routeTree,
  context: {
    queryClient,
  },
  defaultPreload: 'intent',
  defaultPreloadStaleTime: 0,
  defaultErrorComponent: DefaultCatchBoundary,
  defaultNotFoundComponent: () => <NotFound />,
})

declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router
  }
}

interface IGlobalContext {
  signedInAs: User | undefined;
}

export const GlobalContext = createContext<IGlobalContext>({
  signedInAs: undefined
});

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <GlobalContext.Provider value={{ signedInAs: undefined }}>
      <QueryClientProvider client={queryClient}>
        <RouterProvider router={router} />
      </QueryClientProvider>
    </GlobalContext.Provider>
  </StrictMode>,
)
