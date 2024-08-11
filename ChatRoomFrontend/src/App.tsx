/* eslint-disable react/react-in-jsx-scope */
import './App.css'
import { InteractiveDataRow } from './components/dataTable2';

//Todo: No more than one table can be used at the same time
//Todo: Single tables don't work if there are no rows in that table on the server
//Todo: object loses it's class identity after reassembly

export default function App() {
  return (
    <>
      <InteractiveDataRow />
    </>
  )
}
