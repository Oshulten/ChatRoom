/* eslint-disable react/react-in-jsx-scope */
import { createFileRoute } from '@tanstack/react-router'
import Authenticate from '../components/Authenticate';
import DisplayContext from '../components/DisplayContext';

export const Route = createFileRoute('/login')({
  component: () => <>
    <Authenticate authenticationType={"login"} />
    <DisplayContext />
  </>
})
