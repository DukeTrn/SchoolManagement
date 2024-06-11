import Nav from "@/components/nav";
import {
	ResizableHandle,
	ResizablePanel,
	ResizablePanelGroup,
} from "@/components/ui/resizable";
import { sideBarNavs } from "@/constants/nav";

import Logo from "@/assets/Logo.jpg";

interface IMainLayout {
	children: React.ReactNode;
}

const MainLayout = ({ children }: IMainLayout) => {
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
				<ResizablePanel defaultValue={165} minSize={20} maxSize={25}>
					<div className="flex items-center justify-center">
						<img src={Logo} width="60%" />
					</div>
					<Nav links={sideBarNavs} />
				</ResizablePanel>
				<ResizableHandle withHandle />

				<ResizablePanel defaultSize={655} minSize={40}>
					<div className="p-5">{children}</div>
				</ResizablePanel>
			</ResizablePanelGroup>
		</div>
	);
};

export default MainLayout;
