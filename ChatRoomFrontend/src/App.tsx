/* eslint-disable react/react-in-jsx-scope */
import { useState } from 'react';
import './App.css'
import AuthenticateForm, { AuthenticationFields } from './components/authenticateForm'
import User from './types/user';

const baseUrl = "http://localhost:5055/api";

type LoginState = "login" | "createAccount";

async function authorizeUser(fields: AuthenticationFields): Promise<User | undefined> {
  const response = await fetch(`${baseUrl}/ChatUsers/authenticate`, {
    method: "POST",
    body: JSON.stringify(fields),
    headers: {
      "Content-Type": "application/json",
    }
  });

  if (!response.ok || response.status == 204) {
    return;
  }

  return (await response.json()) as User;
}

async function createAccount(fields: AuthenticationFields): Promise<User | undefined> {
  const response = await fetch(`${baseUrl}/ChatUsers/create-account`, {
    method: "POST",
    body: JSON.stringify(fields),
    headers: {
      "Content-Type": "application/json",
    }
  });

  if (!response.ok || response.status == 204) {
    return;
  }

  return (await response.json()) as User;
}

export default function App() {
  const [loginState, setLoginState] = useState<LoginState>("login");
  const [lastUsernameTried, setLastUsernameTried] = useState<string | null>(null);

  let title, switchStateButtonMessage;
  switch (loginState) {
    case "login":
      title = "Welcome back!";
      switchStateButtonMessage = "New here?"
      break;

    case "createAccount":
      title = "Wonderful to see a new face!"
      switchStateButtonMessage = "Already have an account?"
      break;
  }

  function handleSwitchStateClick(): void {
    setLoginState(loginState == "login" ? "createAccount" : "login");
    setLastUsernameTried(null);
  }

  async function handleCreateAccount(fields: AuthenticationFields) {
    const user = await createAccount(fields);
    if (user) {
      console.log("Created account for user: " + JSON.stringify(user));
      return;
    }
    console.log("Failed to create account for user: " + JSON.stringify(fields));
  }

  async function handleLogin(fields: AuthenticationFields) {
    const user = await authorizeUser(fields);
    if (user) {
      console.log("Logged in as " + JSON.stringify(user));
      return;
    }
    console.log("Failed to login as " + JSON.stringify(fields));
    setLastUsernameTried(fields.username);
  }

  return (
    <div className="p-4 rounded-lg bg-slate-800 border-2">
      <h2>{title}</h2>
      {loginState == "login" &&
        <AuthenticateForm
          submitLabel='Login'
          onSuccess={(fields) => handleLogin(fields)}
          errorMessage={lastUsernameTried ? "Sorry, that username or password is invalid" : undefined} />
      }
      {loginState == "createAccount" &&
        <AuthenticateForm
          submitLabel='Create Account'
          onSuccess={(fields) => handleCreateAccount(fields)}
          errorMessage={lastUsernameTried ? "Sorry, this username is already taken" : undefined} />
      }
      <button onClick={() => handleSwitchStateClick()}>{switchStateButtonMessage}</button>
    </div>)
}
