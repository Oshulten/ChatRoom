/* eslint-disable react/react-in-jsx-scope */
import { createFileRoute } from '@tanstack/react-router'
import Authenticate from '../components/Authenticate';

export const Route = createFileRoute('/createAccount')({
  component: () => <Authenticate authenticationType={"create"} />
})