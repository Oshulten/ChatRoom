import React, { useState } from 'react'
import './App.css'
import { StringDataCell, lengthPattern } from './components/DataTable'

function App() {
  const [value, setValue] = useState("123");
  return (
    <>
      <StringDataCell value={value} onChange={(newValue) => {
        setValue(newValue);
        console.log("New value: " + newValue);
      }
      } validationPattern={lengthPattern(0, 4)}></StringDataCell>
    </>
  )
}

export default App
