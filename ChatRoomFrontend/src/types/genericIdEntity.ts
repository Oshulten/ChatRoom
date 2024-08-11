import { InteractiveDataCellSupportedTypes } from '../components/dataTable2';

export interface GenericIdEntity {
  id: string
  [key: string]: InteractiveDataCellSupportedTypes
}