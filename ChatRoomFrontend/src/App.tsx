import React, { useState } from 'react'
import './App.css'
import { StringDataCell } from './components/DataTable'
import { lengthPattern } from './utilities/regexPatterns';


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
