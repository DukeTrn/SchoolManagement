import CommonInput from "@/components/input/CommonInput";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import { toast } from "@/components/ui/use-toast";
import React, { useState } from "react";
import { useAppSelector } from "@/redux/store";
import { useNavigate } from "react-router-dom";
import path from "@/constants/path";
import { changePassword } from "@/apis/auth.api";

const ChangePassword = () => {
	const [oldPassword, setOldPassword] = useState("");
	const [password, setPassword] = useState("");
	const navigate = useNavigate();
	const accoundId = useAppSelector((state) => state.users.accoundId);

	const handleLogin = () => {
		changePassword(
			{
				oldPassword: oldPassword,
				newPassword: password,
				confirmPassword: password,
			},
			accoundId as string
		)
			.then(() => {
				toast({
					title: "Thông báo:",
					description: "Đổi mật khẩu thành công",
					className: "border-2 border-green-500 p-4",
				});
				navigate(path.home);
			})
			.catch(() => {
				toast({
					title: "Thông báo:",
					description: "Mật khẩu cũ không đúng!",
					variant: "destructive",
				});
			});
	};
	return (
		<div className="h-screen w-full">
			<div className="flex h-full items-center justify-center">
				<div className="w-[500px]">
					<div className="text-2xl font-semibold">ĐỔI MẬT KHẨU</div>
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
