import Nav from "@/components/nav";
import {
	ResizableHandle,
	ResizablePanel,
	ResizablePanelGroup,
} from "@/components/ui/resizable";
import { sideBarNavs } from "@/constants/nav";

import Logo from "@/assets/Logo.jpg";
import { IAppState, useAppSelector } from "@/redux/store";

interface IMainLayout {
	children: React.ReactNode;
}

const MainLayout = ({ children }: IMainLayout) => {
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
					<div className="flex items-center justify-center">
						<img src={Logo} width="40%" />
					</div>
					<Nav links={navs} />
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
