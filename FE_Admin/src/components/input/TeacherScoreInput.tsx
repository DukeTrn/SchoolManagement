/* eslint-disable @typescript-eslint/no-explicit-any */
import { InputHTMLAttributes, useState } from "react";
import type { RegisterOptions, UseFormRegister } from "react-hook-form";
import { Input } from "../ui/input";

interface IInputProps extends InputHTMLAttributes<HTMLInputElement> {
	errorMessage?: string;
	className?: string;
	classNameInput?: string;
	classNameError?: string;
	register?: UseFormRegister<any>;
	rules?: RegisterOptions;
}

const TeacherScoreInput = ({
	errorMessage,
	className,
	name,
	rules,
	register,
	classNameInput,
	classNameError = "mt-1 min-h-[1.25rem] text-sm text-red-600",
	...rest
}: IInputProps) => {
	const registerProps = register && name ? register(name, rules) : {};

	const handleType = (): React.HTMLInputTypeAttribute => {
		return rest.type as React.HTMLInputTypeAttribute;
	};

	return (
		<div className={className}>
			<Input
				required
				{...rest}
				className={classNameInput}
				{...registerProps}
				type={handleType()}
			/>
			<div className={classNameError}>{errorMessage}</div>
		</div>
	);
};

export default TeacherScoreInput;
