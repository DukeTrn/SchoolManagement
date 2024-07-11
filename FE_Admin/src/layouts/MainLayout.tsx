import Nav from "@/components/nav";
import {
	ResizableHandle,
	ResizablePanel,
	ResizablePanelGroup,
} from "@/components/ui/resizable";
import { sideBarNavs } from "@/constants/nav";

import Logo from "@/assets/Logo.jpg";
import { IAppState, useAppDispatch, useAppSelector } from "@/redux/store";
import { Button } from "@/components/ui/button";
import { useNavigate } from "react-router-dom";
import { setUserAccount } from "@/redux/reducers/userSlice";
import path from "@/constants/path";

interface IMainLayout {
	children: React.ReactNode;
}

const MainLayout = ({ children }: IMainLayout) => {
	const navigation = useNavigate();
	const dispatch = useAppDispatch();
	const { role } = useAppSelector((state: IAppState) => state.users);
	const navs = sideBarNavs?.filter(
		(i) => i.role.includes(role!) || i.role.includes("All")
	);
	return (
		<div className="h-screen">
			<ResizablePanelGroup
				direction="horizontal"
				onLayout={(sizes: number[]) => {
					document.cookie = `react-resizable-panels:layout=${JSON.stringify(
						sizes
					)}`;
				}}
				className="items-stretch"
			>
				<ResizablePanel defaultSize={150} minSize={15} maxSize={20}>
					<div className="flex h-full flex-col justify-between">
						<div>
							<div className="flex items-center justify-center">
								<img src={Logo} width="40%" />
							</div>
							<Nav links={navs} />
						</div>

						<div className="px-2">
							<Button
								variant="outline"
								className="mb-5 w-full"
								onClick={() => {
									navigation(path.changePassword);
								}}
							>
								Đổi mật khẩu
							</Button>
							<Button
								variant="outline"
								className="mb-5 w-full"
								onClick={() => {
									localStorage.removeItem("accoundId");
									localStorage.removeItem("role");
									dispatch(
										setUserAccount({
											accoundId: null,
											role: null,
										})
									);
									navigation(path.login);
								}}
							>
								Đăng xuất
							</Button>
						</div>
					</div>
				</ResizablePanel>
				<ResizableHandle withHandle />

				<ResizablePanel defaultSize={655} minSize={40}>
					<div className="max-h-screen overflow-auto p-5">
						{children}
					</div>
				</ResizablePanel>
			</ResizablePanelGroup>
		</div>
	);
};

export default MainLayout;
