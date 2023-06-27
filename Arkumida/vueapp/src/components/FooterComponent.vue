<!-- Site footer -->
<script setup>
    import VersionInfo from './VersionInfo.vue'
    import SiteFriends from "./SiteFriends.vue";
    import SitePartners from "./SitePartners.vue";
    import FooterSiteInfo from "./FooterSiteInfo.vue";

</script>

<template>
    <footer class="footer">
        <div class="horizontal-flex flex-center footer-container">
            
            <div class="footer-info-column centered">
                <div>
                    <FooterSiteInfo />
                </div>
            </div>

            <div class="footer-useful-column centered">
                <div class="footer-column-header">
                    Полезное
                </div>
                <div>
                    <div class="footer-block centered">
                        <a href="/allImages" title="Все рисунки к рассказам">Все рисунки</a> к рассказам.
                    </div>

                    <div class="footer-block centered">
                        Наши <a href="/ourBanners" title="Наши кнопки">кнопки</a>.
                    </div>

                    <div class="footer-block centered">
                        Наши <a href="/ourPrograms" title="Наши программы">программы</a>.
                    </div>
                </div>
            </div>
            
            <div class="footer-developers-column centered">
                <div class="footer-column-header">
                    Разработчики
                </div>
                <div>
                    <div class="footer-block">
                        Изначальный дизайн и php-программирование - Şeti
                    </div>

                    <div class="footer-block">
                        Редизайн, программирование - <a href="https://fossa.life" title="Блог Белой Фоссы">Белая Фосса</a>
                    </div>

                    <div class="footer-block">
                        Идея сайта и перевод текстов - <a href="mailto:redgerra@rambler.ru">Redgerra</a>
                    </div>
                </div>
            </div>
            
            <div class="footer-friends-column centered">
                <div class="footer-column-header">
                    Друзья сайта
                </div>
                <div>
                    <SiteFriends />
                </div>
            </div>
            
            <div class="footer-partners-column centered">
                <div class="footer-column-header">
                    Реклама
                </div>
                <div>
                    <SitePartners />
                </div>
            </div>
        </div>

        <!-- Version info string -->
        <VersionInfo />
    </footer>
</template>