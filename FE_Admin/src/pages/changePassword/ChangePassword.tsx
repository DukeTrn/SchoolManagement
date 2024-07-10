import { login } from "@/apis/login.api";
import CommonInput from "@/components/input/CommonInput";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import { toast } from "@/components/ui/use-toast";
import React, { useState } from "react";
import { useAppDispatch } from "@/redux/store";
import { useNavigate } from "react-router-dom";
import { setUserAccount } from "@/redux/reducers/userSlice";
import path from "@/constants/path";

const ChangePassword = () => {
	const [oldPassword, setOldPassword] = useState("");
	const [password, setPassword] = useState("");
	const dispatch = useAppDispatch();
	const navigate = useNavigate();

	const handleLogin = () => {
		login({
			username: oldPassword,
			password: password,
		})
			.then((res) => {
				const data = res?.data;
				dispatch(
					setUserAccount({
						accoundId: data?.accoundId as string,
						role: data?.role,
					})
				);
				localStorage.setItem("accoundId", data?.accoundId);
				localStorage.setItem("role", data?.role);
				navigate(path.home);
			})
			.catch(() => {
				toast({
					title: "Thông báo:",
					description: "Tài khoản hoặc mật khẩu không đúng!",
					variant: "destructive",
				});
			});
	};
	return (
		<div className="h-screen w-full">
			<div className="flex h-full items-center justify-center">
				<div className="w-[500px]">
					<div className="text-2xl font-semibold">ĐĂNG NHẬP</div>
					<div className="mt-4">
						<Label htmlFor="oldPassword" className="required">
							Mật khẩu cũ:
						</Label>
						<CommonInput
							className="mt-[2px]"
							name="oldPassword"
							onChange={(
								e: React.ChangeEvent<HTMLInputElement>
							) => setOldPassword(e.target?.value)}
						/>
					</div>
					<div>
						<Label htmlFor="password" className="required">
							Mật khẩu mới:
						</Label>
						<CommonInput
							className="relative mt-[2px]"
							name="password"
							type="password"
							onChange={(
								e: React.ChangeEvent<HTMLInputElement>
							) => setPassword(e.target?.value)}
						/>
					</div>
					<div className="mt-4">
						<Button onClick={handleLogin}>Đăng nhập</Button>
					</div>
				</div>
			</div>
		</div>
	);
};

export default ChangePassword;
