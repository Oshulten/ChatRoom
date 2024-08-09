import React, { useState } from 'react'
import './App.css'
import { UserTable, PrimitiveDataCell, PrimitiveDataRow } from './components/DataTable'
import { PrimitiveType } from './utilities/casting';


function App() {
  const [booleanState, setBooleanState] = useState<PrimitiveType>(false);
  const [numberState, setNumberState] = useState<PrimitiveType>(1);
  const [stringState, setStringState] = useState<PrimitiveType>("my state");

  return (
    <>
      <UserTable></UserTable>
      {/* <PrimitiveDataCell value={booleanState} onChange={(value) => {
        setBooleanState(value);
        console.log("boolean value: " + value)
      }} />
      <PrimitiveDataCell value={numberState} onChange={(value) => setNumberState(value)} />
      <PrimitiveDataCell value={stringState} onChange={(value) => setStringState(value)} />
      <p>{JSON.stringify({ booleanState: booleanState, numberState: numberState, stringState: stringState })}</p> */}
      <PrimitiveDataRow />
    </>
  )
}

export default App
