import { useEffect, useRef, useState } from "react";

/* eslint-disable react/react-in-jsx-scope */
export function Login() {
    const [userId, setUserId] = useState<string | null>(null);
    const modalRef = useRef<HTMLDialogElement>(null);
    const userNameRef = useRef<HTMLInputElement>(null);
    const passwordRef = useRef<HTMLInputElement>(null);

    useEffect(() => {
        if (modalRef.current) {
            modalRef.current.showModal();
        }
    }, []);

    const loginAttempt = async () => {
        if (userNameRef.current && passwordRef.current) {
            console.log(`Username = ${userNameRef.current.value}`);
            console.log(`Password = ${passwordRef.current.value}`);
        }
    }

    return (
        <>
            <dialog ref={modalRef} id="my_modal_1" className="modal">
                <div className="modal-box">
                    <h3 className="font-bold text-lg">Welcome to Chat Spaces!</h3>
                    <div className="modal-action place-content-center">
                        <form method="dialog">
                            <div>
                                <input ref={userNameRef} type="text" placeholder="User name" className="input input-bordered w-full max-w-xs" />
                            </div>
                            <div>
                                <input ref={passwordRef} type="password" placeholder="Password" className="input input-bordered w-full max-w-xs" />
                            </div>
                            <br />
                            <button className="btn" onClick={loginAttempt}>Login</button>
                        </form>
                    </div>
                </div>
            </dialog>
        </>
    );
}