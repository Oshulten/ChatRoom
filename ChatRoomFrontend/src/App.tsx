import React, { useState } from 'react'
import './App.css'
import { PrimitiveDataRow } from './components/dataTable'
import { GenericIdEntity } from './types/identifiable';

interface SampleEntity extends GenericIdEntity {
  numberValue: number,
  booleanValue: boolean,
  stringValue: string
}


function App() {
  const [entity, setEntity] = useState<SampleEntity>({
    id: "3153",
    numberValue: 125,
    booleanValue: true,
    stringValue: "monkey"
  });

  return (
    <>
      <PrimitiveDataRow entity={entity} handleChange={(newValue) => setEntity(newValue)} />
      <p>{JSON.stringify(entity)}</p>
    </>
  )
}

export default App
