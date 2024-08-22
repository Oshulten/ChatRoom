import { Space, User } from "../api/types"

export interface ApplicationContext {
    currentUser?: User
    currentSpace?: Space
}

export const defaultApplicationContext = {
    currentUser: undefined,
    currentSpace: undefined
}