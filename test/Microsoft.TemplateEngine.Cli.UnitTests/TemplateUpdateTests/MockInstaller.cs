// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.TemplateEngine.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.TemplateEngine.Cli.UnitTests.TemplateUpdateTests
{
    public class MockInstaller : IInstaller
    {
        public MockInstaller()
        {
            Installed = new HashSet<string>();
        }

        public MockInstaller(IEnumerable<string> initiallyInstalled)
            :this()
        {
            InstallPackages(initiallyInstalled);
        }

        public HashSet<string> Installed { get; private set; }

        public void InstallPackages(IEnumerable<string> installationRequests) => InstallPackages(installationRequests, null, false, false);

        // unconditionally adds the installation requests to the _installed list.
        public void InstallPackages(IEnumerable<string> installationRequests, IList<string> nuGetSources) => InstallPackages(installationRequests, nuGetSources, false, false);

        public void InstallPackages(IEnumerable<string> installationRequests, IList<string> nuGetSources, bool debugAllowDevInstall) => InstallPackages(installationRequests, nuGetSources, debugAllowDevInstall, false);

        public void InstallPackages(IEnumerable<string> installationRequests, IList<string> nuGetSources, bool debugAllowDevInstall, bool interactive)
        {
            Installed.UnionWith(installationRequests);
        }

        public void InstallPackages(IEnumerable<InstallationRequest> installationRequests, IList<string> nuGetSources = null, bool debugAllowDevInstall = false, bool interactive = false)
        {
            Installed.UnionWith(installationRequests.Select(r => r.InstallString));
        }

        // Removes the uninstallRequests from the _installed list.
        // Any that aren't in the list are reported in the return value of uninstall failures.
        public IEnumerable<string> Uninstall(IEnumerable<string> uninstallRequests)
        {
            IList<string> uninstallFailures = new List<string>();

            foreach (string toUninstall in uninstallRequests)
            {
                if (!Installed.Remove(toUninstall))
                {
                    uninstallFailures.Add(toUninstall);
                }
            }

            return uninstallFailures;
        }
    }
}
