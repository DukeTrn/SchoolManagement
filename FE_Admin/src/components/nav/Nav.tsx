import { NavLink, useLocation } from "react-router-dom";
import { LucideIcon } from "lucide-react";
import { cn } from "@/lib/utils";
import { buttonVariants } from "@/components/ui/button";

interface INavProps {
	links: {
		title: string;
		icon: LucideIcon;
		path?: string;
		variant?: string;
	}[];
}

const Nav = (props: INavProps) => {
	const { links } = props;
	return (
		<div className="flex flex-col gap-4 py-2">
			<div className="grid gap-1 px-2">
				{links?.map((item) => {
					const isActive = location?.pathname.includes(
						item?.path as string
					);
					return (
						<NavLink
							key={item.title}
							to={`${item?.path}`}
							className={() =>
								cn(
									buttonVariants({
										variant: isActive ? "default" : "ghost",
										size: "sm",
									}),
									isActive
										? "justify-start text-sm text-white"
										: "justify-start text-sm text-black"
								)
							}
						>
							<item.icon className="mr-3 h-4 w-4" />
							{item.title}
						</NavLink>
					);
				})}
			</div>
		</div>
	);
};

export default Nav;
