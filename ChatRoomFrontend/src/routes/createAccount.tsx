/* eslint-disable react/react-in-jsx-scope */
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/createAccount')({
  component: () => <div>Hello /createAccount!</div>
})