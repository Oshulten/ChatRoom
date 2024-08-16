/* eslint-disable react/react-in-jsx-scope */
import { createFileRoute } from '@tanstack/react-router'
import { useContext } from 'react';
import { GlobalContext } from '../main';

export const Route = createFileRoute('/spaces')({
  component: () => <div>Hello /spaces!</div>
});

export default function Spaces() {
  const globalContext = useContext(GlobalContext);
  let signedInInfo;
  if (globalContext.signedInAs) {
    signedInInfo = (
      <>
        <h2>{`Signed in as ${globalContext.signedInAs.alias}`}</h2>
        <button>Sign out</button>
      </>
    );
  }
  else {
    signedInInfo = (
      <>
        <h2>{`Not signed in`}</h2>
      </>
    );
  }
  return signedInInfo;
}