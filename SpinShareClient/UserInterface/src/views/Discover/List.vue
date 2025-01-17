<template>
    <AppLayout>
        <SpinTabBar
            :selected="currentTab"
            :tabs="[
                t('discover.tabBar.new'),
                t('discover.tabBar.updated'),
                t('discover.tabBar.hotThisWeek'),
                t('discover.tabBar.hotThisMonth'),
            ]"
            @change="handleTabChange"
        />
        <transition name="default">
            <section
                v-if="charts"
                class="view-discover-new"
            >
                <section>
                    <ChartList :charts="charts" />

                    <SpinHeader>
                        <SpinButton
                            icon="arrow-left"
                            :disabled="currentPage < 1"
                            @click="navigatePrevious"
                        />
                        <SpinButton
                            :label="
                                t('general.pagination.page', [
                                    parseInt(currentPage) + 1,
                                ])
                            "
                            :disabled="true"
                            color="transparent"
                        />
                        <SpinButton
                            icon="arrow-right"
                            :disabled="charts.length < 12"
                            @click="navigateNext"
                        />
                    </SpinHeader>
                </section>
            </section>
        </transition>
        <section
            v-if="!charts"
            class="view-discover-loading"
        >
            <SpinLoader />
        </section>
    </AppLayout>
</template>

<script setup>
import { ref, onMounted, nextTick, inject } from 'vue';
import AppLayout from '@/layouts/AppLayout.vue';
import ChartList from '@/components/Common/ChartList.vue';
import {
    getHotThisMonthCharts,
    getHotThisWeekCharts,
    getNewCharts,
    getUpdatedCharts,
} from '@/api/api';
import router from '@/router';
import { useRoute } from 'vue-router';
import SpinButton from '@/components/Common/SpinButton.vue';
import SpinTabBar from '@/components/Common/SpinTabBar.vue';

import { useI18n } from 'vue-i18n';
import { Buttons, focusableElements } from '@/modules/useGamepad';
const emitter = inject('emitter');
const { t } = useI18n();

const getTabIndex = (tabName) => {
    if (tabName === 'new') return 0;
    if (tabName === 'updated') return 1;
    if (tabName === 'hotThisWeek') return 2;
    if (tabName === 'hotThisMonth') return 3;

    return 0;
};

const getTabName = (tabIndex) => {
    if (tabIndex === 0) return 'new';
    if (tabIndex === 1) return 'updated';
    if (tabIndex === 2) return 'hotThisWeek';
    if (tabIndex === 3) return 'hotThisMonth';

    return 'new';
};

const route = useRoute();
const charts = ref(null);
const currentTab = ref(getTabIndex(route.params.tab));
const currentPage = ref(route.params.page);

onMounted(async () => {
    if (currentTab.value === 0)
        charts.value = await getNewCharts(currentPage.value);
    if (currentTab.value === 1)
        charts.value = await getUpdatedCharts(currentPage.value);
    if (currentTab.value === 2)
        charts.value = await getHotThisWeekCharts(currentPage.value);
    if (currentTab.value === 3)
        charts.value = await getHotThisMonthCharts(currentPage.value);

    if (window.spinshare.settings.IsConsole) {
        // Select first Element
        await nextTick();
        const firstFocusableElement = document.body
            .querySelector('.view-discover-new')
            .querySelector(focusableElements);

        if (firstFocusableElement) {
            firstFocusableElement.focus();
        }

        // Controller Hints
        let controllerHintItems = [];

        controllerHintItems.push({
            input: Buttons.A,
            label: t('general.select'),
            onclick: () => {
                const focussedElement = document.body.querySelector('*:focus');
                if (focussedElement) {
                    focussedElement.click();
                }
            },
        });

        emitter.emit('console-update-controller-hints', {
            showMenu: true,
            showBack: true,
            items: controllerHintItems,
        });
    }
});

const handleTabChange = (tabIndex) => {
    router.push({
        path: '/discover/' + getTabName(tabIndex) + '/0',
    });
};

const navigatePrevious = () => {
    if (currentPage.value < 1) return;

    currentPage.value--;
    router.push({
        path:
            '/discover/' +
            getTabName(currentTab.value) +
            '/' +
            currentPage.value,
    });
};
const navigateNext = () => {
    if (charts.value.length < 12) return;

    currentPage.value++;
    router.push({
        path:
            '/discover/' +
            getTabName(currentTab.value) +
            '/' +
            currentPage.value,
    });
};
</script>

<style lang="scss" scoped>
.view-discover-loading {
    min-height: 50%;
    display: flex;
    justify-content: center;
    align-items: center;
}
.view-discover-new {
    padding: 40px;
    display: flex;
    flex-direction: column;
    gap: 40px;

    & > section {
        display: flex;
        flex-direction: column;
        gap: 10px;
    }
}
</style>
