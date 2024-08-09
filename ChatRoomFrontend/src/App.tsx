import React, { useState } from 'react'
import './App.css'
import { UserTable, GenericCell } from './components/DataTable'
import { PrimitiveType } from './utilities/casting';


function App() {
  const [booleanState, setBooleanState] = useState<PrimitiveType>(false);
  const [numberState, setNumberState] = useState<PrimitiveType>(1);
  const [stringState, setStringState] = useState<PrimitiveType>("my state");

  return (
    <>
      <UserTable></UserTable>
      <GenericCell value={booleanState} onChange={(value) => {
        setBooleanState(value);
        console.log("boolean value: "+value)}} />
      <GenericCell value={numberState} onChange={(value) => setNumberState(value)} />
      <GenericCell value={stringState} onChange={(value) => setStringState(value)} />
      <p>{JSON.stringify({ booleanState: booleanState, numberState: numberState, stringState: stringState })}</p>
    </>
  )
}

export default App
