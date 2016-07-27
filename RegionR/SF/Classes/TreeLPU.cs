using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary.SF.Lists;
using ClassLibrary.SF.Models;

namespace RegionR.SF
{
    public static class TreeLPU
    {
        public static TreeNode GetRoot(Organization organization, TreeNode root = null)
        {
            TreeNode node = new TreeNode(organization.ShortName);
            node.Name = organization.ID.ToString();

            node.ImageIndex = (organization is LPU) ? 1 : 0;
            node.SelectedImageIndex = (organization is LPU) ? 1 : 0;

            if (root != null)
                root.Nodes.Add(node);

            OrganizationList organizationList = OrganizationList.GetUniqueInstance();

            var list = organizationList.GetChildList(organization);

            foreach (var item in list)
                GetRoot(item, node);

            return node;
        }
    }
}
